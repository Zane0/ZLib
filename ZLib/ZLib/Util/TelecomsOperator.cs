
namespace ZLib.Util
{
	/// <summary>
	/// 中国电信运营商
	/// </summary>
	public enum TelecomsOperator : byte
	{
		/// <summary>
		/// 非手机号码
		/// </summary>
		UNKnown,
		/// <summary>
		/// 中国移动
		/// </summary>
		ChinaMobile,
		/// <summary>
		/// 中国联通
		/// </summary>
		ChinaUnicom,
		/// <summary>
		/// 中国电信
		/// </summary>
		ChinaTelecom,
		/// <summary>
		/// 其他运营商
		/// </summary>
		Other
	}
}
