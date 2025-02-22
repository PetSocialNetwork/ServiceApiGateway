namespace Service_ApiGateway.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddServiceClient<PetSocialNetwork.ServiceAuth.IAuthClient, PetSocialNetwork.ServiceAuth.AuthClient>("AuthService");
            services.AddServiceClient<PetSocialNetwork.ServiceChat.IChatClient, PetSocialNetwork.ServiceChat.ChatClient>("ChatService");
            services.AddServiceClient<PetSocialNetwork.ServiceChat.IMessageClient, PetSocialNetwork.ServiceChat.MessageClient>("ChatService");
            services.AddServiceClient<PetSocialNetwork.ServiceComments.ICommentClient, PetSocialNetwork.ServiceComments.CommentClient>("CommentService");
            services.AddServiceClient<PetSocialNetwork.ServiceNotification.INotificationClient, PetSocialNetwork.ServiceNotification.NotificationClient>("NotificationService");
            services.AddServiceClient<PetSocialNetwork.ServiceUser.IUserProfileClient, PetSocialNetwork.ServiceUser.UserProfileClient>("UserService");
            services.AddServiceClient<PetSocialNetwork.ServicePet.IPetProfileClient, PetSocialNetwork.ServicePet.PetProfileClient>("PetService");
            services.AddServiceClient<PetSocialNetwork.ServicePhoto.IPersonalPhotoClient, PetSocialNetwork.ServicePhoto.PersonalPhotoClient>("PhotoService");
            services.AddServiceClient<PetSocialNetwork.ServicePhoto.IPetPhotoClient, PetSocialNetwork.ServicePhoto.PetPhotoClient>("PhotoService");
            services.AddServiceClient<PetSocialNetwork.ServiceFriend.IFriendShipClient, PetSocialNetwork.ServiceFriend.FriendShipClient>("FriendService");

            return services;
        }
    }
}
