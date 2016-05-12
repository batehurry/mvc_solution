using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CommonUtil
{
    /// <summary>
    /// 加密帮助类 
    /// </summary>
    public class CryptoHelper
    {
        #region 使用Get传输替换关键字符为全角和半角转换
        /// <summary>
        /// 使用Get传输替换关键字符为全角
        /// </summary>
        /// <param name="UrlParam"></param>
        /// <returns></returns>
        public static string UrlParamUrlEncodeRun(string UrlParam)
        {
            UrlParam = UrlParam.Replace("+", "＋");
            UrlParam = UrlParam.Replace("=", "＝");
            UrlParam = UrlParam.Replace("&", "＆");
            UrlParam = UrlParam.Replace("?", "？");
            return UrlParam;
        }

        /// <summary>
        /// 使用Get传输替换关键字符为半角
        /// </summary>
        /// <param name="UrlParam"></param>
        /// <returns></returns>
        public static string UrlParamUrlDecodeRun(string UrlParam)
        {
            UrlParam = UrlParam.Replace("＋", "+");
            UrlParam = UrlParam.Replace("＝", "=");
            UrlParam = UrlParam.Replace("＆", "&");
            UrlParam = UrlParam.Replace("？", "?");
            return UrlParam;
        }
        #endregion

        #region 通过HTTP传递的Base64编码
        /// <summary>
        /// 编码 通过HTTP传递的Base64编码
        /// </summary>
        /// <param name="source">编码前的</param>
        /// <returns>编码后的</returns>
        public static string HttpBase64Encode(string source)
        {
            //空串处理
            if (source == null || source.Length == 0)
            {
                return "";
            }

            //编码
            string encodeString = Convert.ToBase64String(Encoding.UTF8.GetBytes(source));

            //过滤
            encodeString = encodeString.Replace("+", "~");
            encodeString = encodeString.Replace("/", "@");
            encodeString = encodeString.Replace("=", "$");

            //返回
            return encodeString;
        }

        /// <summary>
        /// 解码 通过HTTP传递的Base64解码
        /// </summary>
        /// <param name="source">解码前的</param>
        /// <returns>解码后的</returns>
        public static string HttpBase64Decode(string source)
        {
            //空串处理
            if (source == null || source.Length == 0)
            {
                return "";
            }

            //还原
            string deocdeString = source;
            deocdeString = deocdeString.Replace("~", "+");
            deocdeString = deocdeString.Replace("@", "/");
            deocdeString = deocdeString.Replace("$", "=");

            //Base64解码
            deocdeString = Encoding.UTF8.GetString(Convert.FromBase64String(deocdeString));

            //返回
            return deocdeString;
        }
        #endregion

        #region 获得RSA密钥 加密解密
        /// <summary>
        /// 生成并保存RSA密钥
        /// </summary>
        /// <param name="publicUrl">公钥保存路径</param>
        /// <param name="privateUrl">私钥保存路径</param>
        public static void Create_RSAKey(string publicUrl,string privateUrl)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024);
            using (StreamWriter writer = new StreamWriter(privateUrl))
            {
                //导出私钥
                writer.WriteLine(rsa.ToXmlString(true));
            }
            using (StreamWriter writer = new StreamWriter(publicUrl))
            {
                //导出公钥
                writer.WriteLine(rsa.ToXmlString(false));
            }
        }
        /// <summary>
        /// 加载密钥
        /// </summary>
        /// <param name="rsa">RSACryptoServiceProvider对象</param>
        /// <param name="fileName">文件路径</param>
        public static void Load_RSAKey(RSACryptoServiceProvider rsa, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();
            rsa.FromXmlString(Encoding.UTF8.GetString(data));
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="source">明文</param>
        /// <param name="rsa">RSACryptoServiceProvider对象</param>
        /// <returns>密文</returns>
        public static string RSA_Encrypt(string source, RSACryptoServiceProvider rsa)
        {
            Byte[] PlaintextData = Encoding.UTF8.GetBytes(source);
            int MaxBlockSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制

            if (PlaintextData.Length <= MaxBlockSize)
                return Convert.ToBase64String(rsa.Encrypt(PlaintextData, false));

            using (MemoryStream plaiStream = new MemoryStream(PlaintextData))
            using (MemoryStream crypStream = new MemoryStream())
            {
                Byte[] Buffer = new Byte[MaxBlockSize];
                int BlockSize = plaiStream.Read(Buffer, 0, MaxBlockSize);

                while (BlockSize > 0)
                {
                    Byte[] ToEncrypt = new Byte[BlockSize];
                    Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);

                    Byte[] Cryptograph = rsa.Encrypt(ToEncrypt, false);
                    crypStream.Write(Cryptograph, 0, Cryptograph.Length);

                    BlockSize = plaiStream.Read(Buffer, 0, MaxBlockSize);
                }

                return Convert.ToBase64String(plaiStream.ToArray(), Base64FormattingOptions.None);
            }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="source">密文</param>
        /// <param name="rsa">RSACryptoServiceProvider对象</param>
        /// <returns>原文</returns>
        public static string RSA_Decrypt(string source, RSACryptoServiceProvider rsa)
        {
            Byte[] ciphertextData = Convert.FromBase64String(source);
            int MaxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制

            if (ciphertextData.Length <= MaxBlockSize)
                return Encoding.UTF8.GetString(rsa.Decrypt(ciphertextData, false));

            using (MemoryStream crypStream = new MemoryStream(ciphertextData))
            using (MemoryStream plaiStream = new MemoryStream())
            {
                Byte[] Buffer = new Byte[MaxBlockSize];
                int BlockSize = crypStream.Read(Buffer, 0, MaxBlockSize);

                while (BlockSize > 0)
                {
                    Byte[] ToDecrypt = new Byte[BlockSize];
                    Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);

                    Byte[] Plaintext = rsa.Decrypt(ToDecrypt, false);
                    plaiStream.Write(Plaintext, 0, Plaintext.Length);

                    BlockSize = crypStream.Read(Buffer, 0, MaxBlockSize);
                }

                return Encoding.UTF8.GetString(plaiStream.ToArray());
            }
        }

        #endregion

        #region  MD5加密

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="addKey">附加字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, string addKey, Encoding encoding)
        {
            if (addKey.Length > 0)
            {
                //source = source + addKey;
                source = addKey + source + addKey;
            }

            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] datSource = encoding.GetBytes(source);
            byte[] newSource = MD5.ComputeHash(datSource);
            string byte2String = null;
            for (int i = 0; i < newSource.Length; i++)
            {
                string thisByte = newSource[i].ToString("x");
                if (thisByte.Length == 1)
                    thisByte = "0" + thisByte;
                byte2String += thisByte;
            }
            return byte2String;
        }

        /// <summary>
        /// 标准MD5加密
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source, Encoding encoding)
        {
            return MD5_Encrypt(source, string.Empty, encoding);
        }
        /// <summary>
        /// 标准MD5加密
        /// </summary>
        /// <param name="source">被加密的字符串</param>
        /// <returns></returns>
        public static string MD5_Encrypt(string source)
        {
            return MD5_Encrypt(source, string.Empty, Encoding.Default);
        }


        #endregion

        #region 密码加密
        /// <summary>
        /// 返回使用MD5加密后字符串
        /// </summary>
        /// <param name="strpwd">待加密字符串</param>
        /// <returns>加密后字符串</returns>
        public static string RegUser_MD5_Pwd(string strpwd)
        {
            #region

            string appkey = "fdjf,jkgfkl"; //，。加一特殊的字符后再加密，这样更安全些
            //strpwd += appkey;

            MD5 MD5 = new MD5CryptoServiceProvider();
            byte[] a = System.Text.Encoding.Default.GetBytes(appkey);
            byte[] datSource = System.Text.Encoding.Default.GetBytes(strpwd);
            byte[] b = new byte[a.Length + 4 + datSource.Length];

            int i;
            for (i = 0; i < datSource.Length; i++)
            {
                b[i] = datSource[i];
            }

            b[i++] = 163;
            b[i++] = 172;
            b[i++] = 161;
            b[i++] = 163;

            for (int k = 0; k < a.Length; k++)
            {
                b[i] = a[k];
                i++;
            }

            byte[] newSource = MD5.ComputeHash(b);
            string byte2String = null;
            for (i = 0; i < newSource.Length; i++)
            {
                string thisByte = newSource[i].ToString("x");
                if (thisByte.Length == 1) thisByte = "0" + thisByte;
                byte2String += thisByte;
            }
            return byte2String;

            #endregion
        }
        #endregion

        #region  DES 加解密
        /// <summary>
        /// DES加解密的默认密钥
        /// </summary>
        public static string Key
        {
            get
            {
                return "!@#ASD12";
            }
        }
        /// <summary>
        /// Desc加密 Encoding.Default
        /// </summary>
        /// <param name="source">待加密字符</param>
        /// <param name="key">密钥</param>
        /// <returns>string</returns>
        public static string DES_Encrypt(string source, string key)
        {
            if (string.IsNullOrEmpty(source))
                return null;
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //把字符串放到byte数组中  
            byte[] inputByteArray = Encoding.Default.GetBytes(source);

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            //            des.Key = UTF8Encoding.UTF8.GetBytes(key);
            //            des.IV  = UTF8Encoding.UTF8.GetBytes(key);
            des.Key = Encoding.Default.GetBytes(key);
            des.IV = Encoding.Default.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }

            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// 使用默认key 做 DES加密 Encoding.Default
        /// </summary>
        /// <param name="source">明文</param>
        /// <returns>密文</returns>
        public static string DES_Encrypt(string source)
        {

            return DES_Encrypt(source, "!@#ASD12");
        }
        /// <summary>
        /// 使用默认key 做 DES解密 Encoding.Default
        /// </summary>
        /// <param name="source">密文</param>
        /// <returns>明文</returns>
        public static string DES_Decrypt(string source)
        {
            return DES_Decrypt(source, "!@#ASD12");
        }

        /// <summary>
        /// DES解密 Encoding.Default
        /// </summary>
        /// <param name="source">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string DES_Decrypt(string source, string key)
        {
            if (string.IsNullOrEmpty(source))
                return null;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //将字符串转为字节数组  
            byte[] inputByteArray = new byte[source.Length / 2];
            for (int x = 0; x < source.Length / 2; x++)
            {
                int i = (Convert.ToInt32(source.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = UTF8Encoding.UTF8.GetBytes(key);
            des.IV = UTF8Encoding.UTF8.GetBytes(key);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region SHA1加密

        /// <summary>
        /// SHA1加密，等效于 PHP 的 SHA1() 代码
        /// </summary>
        /// <param name="source">被加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1_Encrypt(string source)
        {
            byte[] temp1 = Encoding.UTF8.GetBytes(source);

            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            byte[] temp2 = sha.ComputeHash(temp1);
            sha.Clear();

            //注意，不能用这个
            //string output = Convert.ToBase64String(temp2); 

            string output = BitConverter.ToString(temp2);
            output = output.Replace("-", "");
            output = output.ToLower();
            return output;
        }
        #endregion

    }
}
