namespace Service_ApiGateway.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddServiceClient<PetSocialNetwork.ServiceAuth.IAuthClient, PetSocialNetwork.ServiceAuth.AuthClient>("AuthService");
            services.AddServiceClient<PetSocialNetwork.ServiceChat.IChatClient, PetSocialNetwork.ServiceChat.ChatClient>("ChatService");
            services.AddServiceClient<PetSocialNetwork.ServiceComments.ICommentClient, PetSocialNetwork.ServiceComments.CommentClient>("CommentService");
            services.AddServiceClient<PetSocialNetwork.ServiceNotification.INotificationClient, PetSocialNetwork.ServiceNotification.NotificationClient>("NotificationService");
            services.AddServiceClient<PetSocialNetwork.ServiceUser.IUserProfileClient, PetSocialNetwork.ServiceUser.UserProfileClient>("UserService");

            return services;
        }
    }
}
