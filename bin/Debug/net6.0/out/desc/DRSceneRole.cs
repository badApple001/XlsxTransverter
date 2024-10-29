// Encoding: System.Text.UTF8Encoding+UTF8EncodingSealed
// Xlsx Name: SceneRole.xlsx
// Bug feedback: isysprey@foxmail.com


public class DRSceneRole : IDRTable
{
	/// <summary>
	/// 编号
	/// </summary>
	public int ID;
	/// <summary>
	/// 角色名
	/// </summary>
	public string Name;
	/// <summary>
	/// 角色介绍
	/// </summary>
	public string Describe;
	/// <summary>
	/// Spine文件
	/// </summary>
	public string Spine;
	/// <summary>
	/// 默认动画Id
	/// </summary>
	public int AnimationId;
	/// <summary>
	/// 主场景Spine高度偏移
	/// </summary>
	public float OffsetY;
	/// <summary>
	/// 妆容背景
	/// </summary>
	public string MakeUpDIYBG;
	/// <summary>
	/// 换装背景
	/// </summary>
	public string ClosetsDIYBG;
	/// <summary>
	/// 妆容DIY角色高度偏移
	/// </summary>
	public float MakeUpRoleOffsetY;
	/// <summary>
	/// 换装DIY角色高度偏移
	/// </summary>
	public float ClosetsRoleOffsetY;
	/// <summary>
	/// DIY角色缩放
	/// </summary>
	public float MakeUpRoleScale;
	/// <summary>
	/// DIY角色缩放
	/// </summary>
	public float ClosetsRoleScale;
	/// <summary>
	/// DIY聚焦参数
	/// </summary>
	public string DIYFocusArg;
	/// <summary>
	/// 妆容房间里需要替换的浴袍皮肤
	/// </summary>
	public int MakeUpSkin;
	public int Get_ID( )
	{
		return ID;
	}
	public void Decode( System.IO.BinaryReader br )
	{
		ID = br.ReadInt32();
		Name = br.ReadString();
		Describe = br.ReadString();
		Spine = br.ReadString();
		AnimationId = br.ReadInt32();
		OffsetY = br.ReadSingle();
		MakeUpDIYBG = br.ReadString();
		ClosetsDIYBG = br.ReadString();
		MakeUpRoleOffsetY = br.ReadSingle();
		ClosetsRoleOffsetY = br.ReadSingle();
		MakeUpRoleScale = br.ReadSingle();
		ClosetsRoleScale = br.ReadSingle();
		DIYFocusArg = br.ReadString();
		MakeUpSkin = br.ReadInt32();
	}
}


