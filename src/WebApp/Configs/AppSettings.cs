namespace IgniteSpotlight.WebApp.Configs
{
    public class AppSettings
    {
        public virtual LinksSettings Links { get; set; }
        public virtual EndpointSettings Endpoints { get; set; }

    }

    public class LinksSettings
    {
        public virtual string Maps { get; set; }
        public virtual string Kakao { get; set; }
        public virtual string Sms { get; set; }
        public virtual string SmsVerify { get; set; }
    }

    public class EndpointSettings
    {
        public virtual string Maps { get; set; }
        public virtual string AccessToken { get; set; }
        public virtual string Profile { get; set; }
    }
}