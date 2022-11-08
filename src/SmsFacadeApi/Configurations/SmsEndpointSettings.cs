namespace IgniteSpotlight.SmsFacadeApi.Configurations
{
    /// <summary>
    /// This represents the app settings entity for Toast SMS endpoints.
    /// </summary>
    public class SmsEndpointSettings
    {
        /// <summary>
        /// Gets or sets the endpoint to send messages.
        /// </summary>
        public virtual string SendMessages { get; set; }
    }
}