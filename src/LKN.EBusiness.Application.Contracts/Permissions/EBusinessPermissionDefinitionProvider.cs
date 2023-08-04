using LKN.EBusiness.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LKN.EBusiness.Permissions;

public class EBusinessPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(EBusinessPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EBusinessPermissions.MyPermission1, L("Permission:MyPermission1"));
         
        var permissionDefinition = myGroup.AddPermission(EBusinessPermissions.Products.Default);

        var permissionDefinition1 = permissionDefinition.AddChild(EBusinessPermissions.Products.Select);
        permissionDefinition1.AddChild(EBusinessPermissions.Products.Price);
        permissionDefinition1.AddChild(EBusinessPermissions.Products.Email);

        permissionDefinition.AddChild(EBusinessPermissions.Products.Update);
        permissionDefinition.AddChild(EBusinessPermissions.Products.Create);
        permissionDefinition.AddChild(EBusinessPermissions.Products.Delete);
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EBusinessResource>(name);
    }
}
