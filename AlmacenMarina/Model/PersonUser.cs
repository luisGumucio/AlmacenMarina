
namespace AlmacenMarina.Model
{
    public class PersonUser
    {
        private User user;
        private Person person;
        private Roles roles;
        private UserRol userPers;

        public PersonUser()
        {
            user = new User();
            person = new Person();
            roles = new Roles();
            userPers = new UserRol();
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        public Person Person
        {
            get { return person; }
            set { person = value; }
        }

        public Roles Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        public UserRol UserPers
        {
            get { return userPers; }
            set { userPers = value; }
        }

    }
}
