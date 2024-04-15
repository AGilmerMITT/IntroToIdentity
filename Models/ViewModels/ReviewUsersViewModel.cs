namespace IntroToIdentity.Models.ViewModels
{
    public class ReviewUsersViewModel
    {
        public IList<string> Roles { get; init; } = [];
        public IList<Member> Members { get; init; } = [];

        public class Member
        {
            public string Name { get; init; } = string.Empty;
            public HashSet<string> Roles { get; init; } = [];
        }
    }
}
