using System;

namespace Acme.ReturnOh
{
	/// <summary>
	/// Oh拓展类
	/// </summary>
	// Token: 0x0200000A RID: 10
	public static class OhEx
	{
		/// <summary>
		/// 获取失败时的Oh值(data为null时)
		/// </summary>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x06000016 RID: 22 RVA: 0x00002290 File Offset: 0x00000490
		public static Oh Success(string msg)
		{
			return OhEx.Success((object)null, msg);
		}

		/// <summary>
		/// 获取成功时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x06000017 RID: 23 RVA: 0x000022AC File Offset: 0x000004AC
		public static Oh Success(this object data, string msg = "成功！")
		{
			return new Oh(200, msg, data);
		}

		/// <summary>
		/// 获取成功时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x06000018 RID: 24 RVA: 0x000022CC File Offset: 0x000004CC
		public static Oh<TEntity> Success<TEntity>(this TEntity data, string msg = "成功！") where TEntity : class, new()
		{
			return new Oh<TEntity>(200, msg, data);
		}

		/// <summary>
		/// 获取成功时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns></returns>
		// Token: 0x06000019 RID: 25 RVA: 0x000022EC File Offset: 0x000004EC
		public static Oh Success(this object data)
		{
			return new Oh(200, "成功！", data);
		}

		/// <summary>
		/// 获取成功时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns></returns>
		// Token: 0x0600001A RID: 26 RVA: 0x00002310 File Offset: 0x00000510
		public static Oh<TEntity> Success<TEntity>(this TEntity data) where TEntity : class, new()
		{
			return new Oh<TEntity>(200, "成功！", data);
		}

		/// <summary>
		/// 获取成功时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="code">状态码</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x0600001B RID: 27 RVA: 0x00002334 File Offset: 0x00000534
		public static Oh Success(this object data, int code = 200, string msg = "成功！")
		{
			return new Oh(code, msg, data);
		}

		/// <summary>
		/// 获取成功时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="code">状态码</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x0600001C RID: 28 RVA: 0x00002350 File Offset: 0x00000550
		public static Oh<TEntity> Success<TEntity>(this TEntity data, int code = 200, string msg = "成功！") where TEntity : class, new()
		{
			return new Oh<TEntity>(code, msg, data);
		}

		/// <summary>
		/// 获取失败时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x0600001D RID: 29 RVA: 0x0000236C File Offset: 0x0000056C
		public static Oh Fail(this object data, string msg = "服务器错误！请联系管理员")
		{
			return new Oh(500, msg, data);
		}

		/// <summary>
		/// 获取失败时的Oh值(data为null时)
		/// </summary>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x0600001E RID: 30 RVA: 0x0000238C File Offset: 0x0000058C
		public static Oh Fail(string msg)
		{
			return OhEx.Fail((object)null, msg);
		}

		/// <summary>
		/// 获取失败时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x0600001F RID: 31 RVA: 0x000023A8 File Offset: 0x000005A8
		public static Oh<TEntity> Fail<TEntity>(this TEntity data, string msg = "服务器错误！请联系管理员") where TEntity : class, new()
		{
			return new Oh<TEntity>(500, msg, data);
		}

		/// <summary>
		/// 获取失败时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns></returns>
		// Token: 0x06000020 RID: 32 RVA: 0x000023C8 File Offset: 0x000005C8
		public static Oh Fail(this object data)
		{
			return new Oh(500, "服务器错误！请联系管理员", data);
		}

		/// <summary>
		/// 获取失败时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <returns></returns>
		// Token: 0x06000021 RID: 33 RVA: 0x000023EC File Offset: 0x000005EC
		public static Oh<TEntity> Fail<TEntity>(this TEntity data) where TEntity : class, new()
		{
			return new Oh<TEntity>(500, "服务器错误！请联系管理员", data);
		}

		/// <summary>
		/// 获取失败时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="code">状态码</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x06000022 RID: 34 RVA: 0x00002410 File Offset: 0x00000610
		public static Oh Fail(this object data, int code = 500, string msg = "服务器错误！请联系管理员")
		{
			return new Oh(code, msg, data);
		}

		/// <summary>
		/// 获取失败时的Oh值
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="code">状态码</param>
		/// <param name="msg">描述</param>
		/// <returns></returns>
		// Token: 0x06000023 RID: 35 RVA: 0x0000242C File Offset: 0x0000062C
		public static Oh<TEntity> Fail<TEntity>(this TEntity data, int code = 500, string msg = "服务器错误！请联系管理员") where TEntity : class, new()
		{
			return new Oh<TEntity>(code, msg, data);
		}

		/// <summary>
		/// 扩展方法，用于创建一个自定义的 Oh 实例，包含指定的数据、状态码和消息。
		/// </summary>
		/// <param name="data">数据对象。</param>
		/// <param name="code">状态码。</param>
		/// <param name="msg">消息。</param>
		/// <returns>包含指定数据、状态码和消息的 Oh 实例。</returns>
		/// <remarks>
		/// 此方法用于创建一个自定义的 Oh 实例，允许用户指定任意类型的数据、状态码和消息，用于特定的业务场景。
		/// </remarks>
		// Token: 0x06000024 RID: 36 RVA: 0x00002448 File Offset: 0x00000648
		public static Oh Custom(this object data, int code, string msg)
		{
			return new Oh(code, msg, data);
		}

		/// <summary>
		/// 扩展方法，用于创建一个自定义的 Oh 实例，包含指定的实体数据、状态码和消息。
		/// </summary>
		/// <typeparam name="TEntity">实体类型。</typeparam>
		/// <param name="data">实体数据。</param>
		/// <param name="code">状态码。</param>
		/// <param name="msg">消息。</param>
		/// <returns>包含指定实体数据、状态码和消息的 Oh 实例。</returns>
		/// <remarks>
		/// 此方法用于创建一个自定义的 Oh 实例，允许用户指定实体数据、状态码和消息，用于特定的业务场景。
		/// 实体类型必须是引用类型（class），并且必须具有无参数的公共构造函数。
		/// </remarks>
		// Token: 0x06000025 RID: 37 RVA: 0x00002464 File Offset: 0x00000664
		public static Oh<TEntity> Custom<TEntity>(this TEntity data, int code, string msg) where TEntity : class, new()
		{
			return new Oh<TEntity>(code, msg, data);
		}

		/// <summary>
		/// 扩展方法，用于将布尔值转换为表示操作成功或失败的 Oh 实例。
		/// </summary>
		/// <param name="isSuccess">布尔值，指示操作是否成功。</param>
		/// <returns>表示操作成功或失败的 Oh 实例。</returns>
		// Token: 0x06000026 RID: 38 RVA: 0x00002480 File Offset: 0x00000680
		public static Oh IsSuccess(this bool isSuccess)
		{
			return isSuccess ? OhEx.Success((object)null, "新增成功") : OhEx.Fail((object)null, "新增失败");
		}
	}
}
