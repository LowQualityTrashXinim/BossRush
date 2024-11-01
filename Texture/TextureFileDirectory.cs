namespace BossRush.Texture {
	static class BossRushTexture {
		public const string CommonTextureStringPattern = "BossRush/Texture/";
		public const string MissingTexture_Folder = "MissingTexture/";

		public const string WHITEDOT = "BossRush/Texture/WhiteDot";
		public const string MISSINGTEXTUREPOTION = "BossRush/Texture/MissingTexturePotion";
		public const string EMPTYBUFF = "BossRush/Texture/EmptyBuff";
		public const string PLACEHOLDERCHEST = "BossRush/Texture/PlaceHolderTreasureChest";
		public const string WHITEBALL = "BossRush/Texture/WhiteBall";
		public const string DIAMONDSWOTAFFORB = "BossRush/Texture/DiamondSwotaffOrb";
		public const string ACCESSORIESSLOT = "Terraria/Images/Inventory_Back7";
		public const string MENU = "BossRush/Texture/UI/menu";
		public const string ACHIEVEMENTMAINBLUE = "BossRush/Texture/UI/mainblue";
		public const string ACHIEVEMENTMAINWHITE = "BossRush/Texture/UI/mainwhite";
		public const string ACHIEVEMENTMENUBLUE = "BossRush/Texture/UI/horizmainblue";
		public const string ACHIEVEMENTMENUWHITE = "BossRush/Texture/UI/horizmainwhite";
		public const string SMALLWHITEBALL = "BossRush/Texture/smallwhiteball";
		public const string EMPTYCARD = "BossRush/Texture/EmptyCard";
		public const string EXAMPLEUI = "BossRush/Texture/ExampleFrame";
		public const string SUPPILESDROP = "BossRush/Texture/SuppliesDrop";
		public const string FOURSTAR = "BossRush/Texture/FourStar";
		public const string CrossSprite = "BossRush/Texture/UI/Cross";
		public const string Lock = "BossRush/Texture/UI/lock";
		public static string Get_MissingTexture(string text) => CommonTextureStringPattern + MissingTexture_Folder + $"{text}MissingTexture";
		public const string MissingTexture_Default = CommonTextureStringPattern + MissingTexture_Folder + "MissingTextureDefault";
	}
}
