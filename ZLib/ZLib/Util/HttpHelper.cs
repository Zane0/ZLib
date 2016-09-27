using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Compilation;
using System.Web.Configuration;

namespace ZLib.Util
{
	public class HttpHelper
	{
		/// <summary>
		/// Post data到url
		/// </summary>
		/// <param name="url">目标url</param>
		/// <param name="data">要post的数据</param>
		/// <returns>服务器响应</returns>
		public static string GetResponseFromHttpPost(string url, string data)
		{
			Encoding encoding = Encoding.GetEncoding(sRequestEncoding);
			byte[] bytesToPost = encoding.GetBytes(data);
			return PostDataToUrl(bytesToPost, url);
		}

		public static string GetResponseBody(HttpWebResponse hwr)
		{
			Stream responseStream = hwr.GetResponseStream();

			#region 读取服务器返回信息
			string stringResponse = string.Empty;
			using (StreamReader responseReader =
				new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
			{
				stringResponse = responseReader.ReadToEnd();
			}
			responseStream.Close();
			#endregion
			return stringResponse;
		}

		/// <summary>
		/// Post data到url
		/// </summary>
		/// <param name="data">要post的数据</param>
		/// <param name="url">目标url</param>
		/// <returns>服务器响应</returns>
		static string PostDataToUrl(byte[] data, string url)
		{
			#region 创建httpWebRequest对象
			WebRequest webRequest = WebRequest.Create(url);
			HttpWebRequest httpRequest = webRequest as HttpWebRequest;
			if (httpRequest == null)
			{
				throw new ApplicationException(
					string.Format("Invalid url string: {0}", url)
					);
			}
			#endregion

			#region 填充httpWebRequest的基本信息
			httpRequest.UserAgent = sUserAgent;
			httpRequest.ContentType = sContentType;
			httpRequest.Method = "POST";
			#endregion

			#region 填充要post的内容
			httpRequest.ContentLength = data.Length;
			Stream requestStream = httpRequest.GetRequestStream();
			requestStream.Write(data, 0, data.Length);
			requestStream.Close();
			#endregion

			#region 发送post请求到服务器并读取服务器返回信息
			Stream responseStream;
			try
			{
				responseStream = httpRequest.GetResponse().GetResponseStream();
			}
			catch (Exception e)
			{
				// log error
				Console.WriteLine(
					string.Format("POST操作发生异常：{0}", e.Message)
					);
				throw e;
			}
			#endregion

			#region 读取服务器返回信息
			string stringResponse = string.Empty;
			using (StreamReader responseReader =
				new StreamReader(responseStream, Encoding.GetEncoding(sResponseEncoding)))
			{
				stringResponse = responseReader.ReadToEnd();
			}
			responseStream.Close();
			#endregion
			return stringResponse;
		}

		const string sUserAgent =
			"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
		const string sContentType =
			"application/x-www-form-urlencoded";
		const string sRequestEncoding = "UTF-8";
		const string sResponseEncoding = "UTF-8";



		private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

		/// <summary>  
		/// 创建GET方式的HTTP请求  
		/// </summary>  
		/// <param name="url">请求的URL</param>  
		/// <param name="timeout">请求的超时时间</param>  
		/// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
		/// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
		/// <returns></returns>  
		public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, CookieCollection cookies)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
			request.Method = "GET";
			request.UserAgent = DefaultUserAgent;
			if (!string.IsNullOrEmpty(userAgent))
			{
				request.UserAgent = userAgent;
			}
			if (timeout.HasValue)
			{
				request.Timeout = timeout.Value;
			}
			if (cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(cookies);
			}
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>  
		/// 创建POST方式的HTTP请求  
		/// </summary>  
		/// <param name="url">请求的URL</param>  
		/// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
		/// <param name="timeout">请求的超时时间</param>  
		/// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
		/// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
		/// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
		/// <returns></returns>  
		public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			if (requestEncoding == null)
			{
				throw new ArgumentNullException("requestEncoding");
			}
			HttpWebRequest request = null;
			//如果是发送HTTPS请求  
			if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
				request = WebRequest.Create(url) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(url) as HttpWebRequest;
			}
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";

			if (!string.IsNullOrEmpty(userAgent))
			{
				request.UserAgent = userAgent;
			}
			else
			{
				request.UserAgent = DefaultUserAgent;
			}

			if (timeout.HasValue)
			{
				request.Timeout = timeout.Value;
			}
			if (cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(cookies);
			}
			//如果需要POST数据  
			if (!(parameters == null || parameters.Count == 0))
			{
				StringBuilder buffer = new StringBuilder();
				int i = 0;
				foreach (string key in parameters.Keys)
				{
					if (i > 0)
					{
						buffer.AppendFormat("&{0}={1}", key, parameters[key]);
					}
					else
					{
						buffer.AppendFormat("{0}={1}", key, parameters[key]);
					}
					i++;
				}
				byte[] data = requestEncoding.GetBytes(buffer.ToString());
				using (Stream stream = request.GetRequestStream())
				{
					stream.Write(data, 0, data.Length);
				}
			}
			return request.GetResponse() as HttpWebResponse;
		}

		/// <summary>  
		/// 创建POST方式的HTTP请求  
		/// </summary>  
		/// <param name="url">请求的URL</param>  
		/// <param name="parameters">随同请求POST的参数名称及参数值字典</param>  
		/// <param name="timeout">请求的超时时间</param>  
		/// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>  
		/// <param name="requestEncoding">发送HTTP请求时所用的编码</param>  
		/// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>  
		/// <returns></returns>  
		public static HttpWebResponse GetHttpPostResponse(string url, string postData, int? timeout, string userAgent, Encoding requestEncoding, CookieCollection cookies)
		{
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentNullException("url");
			}
			if (requestEncoding == null)
			{
				throw new ArgumentNullException("requestEncoding");
			}
			HttpWebRequest request = null;
			//如果是发送HTTPS请求  
			if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
				request = WebRequest.Create(url) as HttpWebRequest;
				request.ProtocolVersion = HttpVersion.Version10;
			}
			else
			{
				request = WebRequest.Create(url) as HttpWebRequest;
			}
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";

			if (!string.IsNullOrEmpty(userAgent))
			{
				request.UserAgent = userAgent;
			}
			else
			{
				request.UserAgent = DefaultUserAgent;
			}

			if (timeout.HasValue)
			{
				request.Timeout = timeout.Value;
			}
			if (cookies != null)
			{
				request.CookieContainer = new CookieContainer();
				request.CookieContainer.Add(cookies);
			}

			byte[] data = requestEncoding.GetBytes(postData);
			using (Stream stream = request.GetRequestStream())
			{
				stream.Write(data, 0, data.Length);
			}
			return request.GetResponse() as HttpWebResponse;
		}

		private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
		{
			return true; //总是接受  
		}

		/// <summary>
		/// 获取网页的返回内容
		/// </summary>
		/// <param name="urlrequest"></param>
		/// <returns></returns>
		public static string GetResponseFromUrl(string urlrequest)
		{
			WebRequest request = WebRequest.Create(urlrequest);
			WebResponse response = request.GetResponse();

			using (Stream responseStream = response.GetResponseStream())
			{
				using (StreamReader reader = new StreamReader(responseStream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		/// <summary>
		/// 获取客户端 Ip
		/// </summary>
		/// <param name="httpRequest"></param>
		/// <returns></returns>
		public static string GetClientIP(HttpRequest httpRequest)
		{
			if (httpRequest.Headers["x-forwarded-for"] == null)
			{
				return httpRequest.UserHostAddress;
			}
			return httpRequest.Headers["x-forwarded-for"];
		}

		/// <summary>
		/// 根据当前 httpHandlers 配置，获取将处理当前 context 的 HttpHandler 的类型
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static Type GetHttpHandlerType(HttpContext context)
		{
			HttpHandlersSection section = context.GetSection("system.web/httpHandlers") as HttpHandlersSection;
			if (section != null)
			{
				HttpHandlerActionCollection _handlers = section.Handlers as HttpHandlerActionCollection;
				if (_handlers != null)
				{
					for (int i = 0; i < _handlers.Count; i++)
					{
						HttpHandlerAction _action = _handlers[i];
						if (IsMatch(_action, context.Request.RequestType, context.Request))
						{
							return BuildManager.GetType(_action.Type, true, false);
						}
					}
				}
			}
			return null;
		}

		/// <summary>
		/// 判断当前请求是否符合配置节规则
		/// </summary>
		/// <param name="action"></param>
		/// <param name="verb"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		private static bool IsMatch(HttpHandlerAction action, string verb, HttpRequest request)
		{
			Wildcard _wcard = new Wildcard(action.Verb.Replace(" ", string.Empty), false);
			WildcardUrl _path = new WildcardUrl(action.Path, true);
			return (_path.IsSuffix(request.Path)
				&& _wcard.IsMatch(verb));
		}
	}
}
