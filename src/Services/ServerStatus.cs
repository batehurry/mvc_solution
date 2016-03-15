using CommonUtil;

/// <summary>
/// 调用状态
/// </summary>
public enum ServerStatus
{
    /// <summary>
    /// 调用成功
    /// </summary>
    [Remark("调用成功")]
    Success = 10000,
    /// <summary>
    /// 口令过期
    /// </summary>
    [Remark("调用失败")]
    Fail = 40001,
};

/// <summary>
/// 登录状态
/// </summary>
public enum SignInStatus
{    
    /// <summary>
    /// Sign in was successful
    /// </summary>
    Success = 0,  
    /// <summary>
    /// User is locked out
    /// </summary>
    LockedOut = 1, 
    /// <summary>
    /// Sign in requires addition verification (i.e. two factor)
    /// </summary>
    RequiresVerification = 2,
    /// <summary>
    /// Sign in failed
    /// </summary>
    Failure = 3
}
