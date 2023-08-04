namespace LKN.EBusiness.Permissions;

public static class EBusinessPermissions
{
    public const string GroupName = "EBusiness";
    // 根据领域模型来取
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";


    public static class Products
    {
        public const string Default = GroupName + ".Products";
        public const string Select = Default + ".Select";
        public const string Price = Select + ".Price";
        public const string Email = Select+".admin@abp.io";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManagePermissions = Default + ".ManagePermissions";
    }

    public static class Orders
    {
        public const string Default = GroupName + ".Orders";
        public const string Select = Default + ".Select";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string ManagePermissions = Default + ".ManagePermissions";
    }
}
