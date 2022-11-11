namespace IgniteSpotlight.WebApp.Configs
{
    public class AppSettings
    {
        public virtual LinksSettings Links { get; set; }
        public virtual EndpointsSettings Endpoints { get; set; }

    }

    public class LinksSettings
    {
        public const string Name = "Links";

        public virtual string Maps { get; set; }
        public virtual string Kakao { get; set; }
        public virtual string Sms { get; set; }
        public virtual string SmsVerify { get; set; }
    }

    public class EndpointsSettings
    {
        public const string Name = "Endpoints";

        public virtual string Maps { get; set; }
        public virtual string AccessToken { get; set; }
        public virtual string Profile { get; set; }
    }
}