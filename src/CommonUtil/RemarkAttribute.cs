using System;

namespace CommonUtil
{
    public class RemarkAttribute : Attribute
    {
        private string _remark;
        public RemarkAttribute(string remark)
        {
            this._remark = remark;
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
