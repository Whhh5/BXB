using System.Collections.Generic;
public partial class MasterData
{
	public Dictionary<ulong, LocalItemData> LocalItemData = new Dictionary<ulong, LocalItemData>();
	public Dictionary<ulong, LocalizeArticleData> LocalizeArticleData = new Dictionary<ulong, LocalizeArticleData>();
	public Dictionary<ulong, LocalizeConstructionData> LocalizeConstructionData = new Dictionary<ulong, LocalizeConstructionData>();
	public Dictionary<ulong, LocalizeRolesData> LocalizeRolesData = new Dictionary<ulong, LocalizeRolesData>();
	public Dictionary<ulong, LocalizeSkillsData> LocalizeSkillsData = new Dictionary<ulong, LocalizeSkillsData>();
	public Dictionary<ulong, LocalizeUIDialogData> LocalizeUIDialogData = new Dictionary<ulong, LocalizeUIDialogData>();
	public Dictionary<ulong, LocalRolesData> LocalRolesData = new Dictionary<ulong, LocalRolesData>();

}
