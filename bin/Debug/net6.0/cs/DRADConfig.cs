﻿// Encoding: System.Text.UTF8Encoding+UTF8EncodingSealed
// @Generated by XlsxTransverter
// Create Time: 2023/9/6 22:38:20
// Xlsx Name: ADConfig.xlsx
// Bug feedback: isysprey@foxmail.com


public class DRADConfig : IDRTable
{
	public int ID; // 编号
	public string LevelRange; // 关卡范围
	public float Interval; // 间隔： 秒
	public int Get_ID( )
	{
		return ID;
	}
	public void Decode( System.IO.BinaryReader br )
	{
		ID = br.ReadInt32();
		LevelRange = br.ReadString();
		Interval = br.ReadSingle();
	}
}

