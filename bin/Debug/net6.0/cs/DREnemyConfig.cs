﻿// Encoding: System.Text.UTF8Encoding+UTF8EncodingSealed
// @Generated by XlsxTransverter
// Create Time: 2023/9/6 22:38:20
// Xlsx Name: EnemyConfig.xlsx
// Bug feedback: isysprey@foxmail.com


public class DREnemyConfig : IDRTable
{
	public int ID; // 编号
	public string roleIds; // 角色ID数组
	public string cellIndex; // 角色位置数组
	public float ATKScale; // 攻击微调 ( 100 % )
	public float HPScale; // 生命值微调 ( 100 % )
	public int Get_ID( )
	{
		return ID;
	}
	public void Decode( System.IO.BinaryReader br )
	{
		ID = br.ReadInt32();
		roleIds = br.ReadString();
		cellIndex = br.ReadString();
		ATKScale = br.ReadSingle();
		HPScale = br.ReadSingle();
	}
}


