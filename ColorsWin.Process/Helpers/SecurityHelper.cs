using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace ColorsWin.Process.Helpers
{

    public class SecurityHelper
    {
        /// <summary>
        /// 判断是否以管理员权限运行本程序
        /// </summary>
        /// <returns></returns>
        public static bool IsRunAsAdmin()
        {
            var id = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(id);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        ///为文件夹添加users，everyone用户组的完全控制权限
        /// </summary>
        /// <param name="dirPath"></param>
        public static void AddSecurity(string dirPath)
        {
#if NET40
            var dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
            {
                return;
            }
            var dirSecurity = dir.GetAccessControl(AccessControlSections.All);

            var inherits = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit;
            //var currentName = WindowsIdentity.GetCurrent().Name; 

            //添加ereryone用户组的访问权限规则 完全控制权限
            var everyoneAccessRule = new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, everyoneAccessRule, out var isModified);

            //添加Users用户组的访问权限规则 完全控制权限
            var usersAccessRule = new FileSystemAccessRule("Users", FileSystemRights.FullControl, inherits, PropagationFlags.None, AccessControlType.Allow);
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersAccessRule, out isModified);

            dir.SetAccessControl(dirSecurity);
#endif
        }

        /// <summary>
        /// 检测是否已经有Everyone 完全控制权限
        /// </summary>
        /// <param name="dirPath"></param>
        public static bool CheckEveryoneAccess(string dirPath)
        {
#if NET40
            string everyoneTag = "S-1-1-0";//
            // everyoneTag = "Everyone"; 

            var accessRules = Directory.GetAccessControl(dirPath).GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (AuthorizationRule role in accessRules)
            {
                if (role.IdentityReference.Value == everyoneTag)
                {
                    var fileAccess = (FileSystemAccessRule)role;
                    if (fileAccess.AccessControlType == AccessControlType.Allow && fileAccess.FileSystemRights.HasFlag(FileSystemRights.FullControl))
                    {
                        return true;
                    }
                }
            }
#endif
            return false;
        }
    }

}
