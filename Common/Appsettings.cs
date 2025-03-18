namespace Test003.Common
{
    public class Appsettings
    {
        private static readonly IConfigurationRoot Config;

        static Appsettings()
        {
            // 加载appsettings.json，并构建IConfigurationRoot
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            Config = builder.Build();
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static string EnableDb => Config["ConnectionStrings:Enable"];

        /// <summary>
        /// 数据库连接串
        /// </summary>
        public static string ConnectionStrings => Config.GetConnectionString(EnableDb);

        /// <summary>
        /// 项目地址
        /// </summary>
        public static string WebSiteUrl => Config["WebSiteUrl"];

        /// <summary>
        /// JWT授权
        /// </summary>
        public static class JwtAuth
        {
            public static string Secret => Config["JwtAuth:Secret"];
            public static string Issuer => Config["JwtAuth:Issuer"];
            public static string Audience => Config["JwtAuth:Audience"];
        }

        /// <summary>
        /// 跨域访问
        /// </summary>
        public static class Cors
        {
            public static string PolicyName => Config["Cors:PolicyName"];
            public static string Urls => Config["Cors:Urls"];
        }

        /// <summary>
        /// //文件存储位置1为本地存储，2为S3存储对应数据库WP_FILES中的Type字段
        /// </summary>
        public static string FileType => Config["FileType"];

        /// <summary>
        /// 会议平台切换————true:腾讯会议,false:企微会议
        /// </summary>
        public static bool MeetingSwitch => Convert.ToBoolean(Config["MeetingSwitch"]);

        /// <summary>
        /// 腾讯会议相关配置
        /// </summary>
        public static class TencentMeeting
        {
            public static string Token => Config["TencentMeeting:Token"];
            public static string AppId => Config["TencentMeeting:AppId"];
            public static string SecretId => Config["TencentMeeting:SecretId"];
            public static string SecretKey => Config["TencentMeeting:SecretKey"];
            public static string SdkId => Config["TencentMeeting:SdkId"];
            public static string DefaultUserid => Config["TencentMeeting:DefaultUserid"];
            public static string BeforeMeetingTime => Config["TencentMeeting:BeforeMeetingTime"];
        }

        /// <summary>
        /// 腾讯云短信配置
        /// </summary>
        public static class TencetenSMS
        {
            public static string SecretId = Config["TencetenSMS:SecretId"];
            public static string SecretKey = Config["TencetenSMS:SecretKey"];
            public static string SmsSdkAppId = Config["TencetenSMS:SmsSdkAppId"];
            public static string SignName = Config["TencetenSMS:SignName"];
            public static string TemplateId = Config["TencetenSMS:TemplateId"];
            public static string EffectiveTime = Config["TencetenSMS:EffectiveTime"];
            public static string Region = Config["TencetenSMS:Region"];
        }

        /// <summary>
        /// 公众号配置
        /// </summary>
        public static class WeChat
        {
            public static string AppID => Config["WeChat:AppID"];
            public static string Secret => Config["WeChat:Secret"];
        }

        /// <summary>
        /// 企业微信相关配置
        /// </summary>
        public static class WeCom
        {
            public static string CorpID => Config["WeCom:CorpID"];
            public static string Secret => Config["WeCom:Secret"];
        }
    }
}
