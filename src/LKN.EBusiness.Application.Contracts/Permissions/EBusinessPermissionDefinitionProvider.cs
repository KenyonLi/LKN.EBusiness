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
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EBusinessResource>(name);
    }
}
