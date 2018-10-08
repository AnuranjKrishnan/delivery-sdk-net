namespace StepBuilder
{
    class Program
    {
        static void Main(string[] args)
        {

            var localUserConfiguration = UserConfigurationBuilder.CreateNewBuilder()
                .SetName(@"aLocalConfiguration")
                .SetFilePath(@"/opt/conf/user.txt")
                .OnLocalhost().Build();



            UserConfiguration remoteUserConfiguration = UserConfigurationBuilder.CreateNewBuilder()
                           .SetName(@"aRemoteConfiguration")
                           .SetFilePath(@"/opt/conf/user.txt")
                           .OnRemoteHost("172.x.x.x")
                           .SetCredentials("user", "password")
                           .Build();
        }
    }

    public class UserConfiguration
    {
        public UserConfiguration(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }


        public string Name { get; private set; }

        public string FilePath { get; private set; }
    }

    public class UserConfigurationBuilder
    {
        public static INameStep CreateNewBuilder()
        {
            return new Steps();
        }

        private UserConfigurationBuilder() { }

        public interface INameStep
        {
            /// <param name="name">Unique identifier for this User Configuration</param>
            IFileStep SetName(string name);
        }

        public interface IFileStep
        {
            /// <param name="filePath">Absolute path of where the User Configuration exists</param>
            IServerStep SetFilePath(string filePath);
        }

        public interface IServerStep
        {
            /// <summary>
            /// The hostname of the server where the User Configuration file is store will be set to "localhost".
            /// </summary>
            IBuildStep OnLocalhost();

            /// <param name="host">The hostname of the server where the User Configuration is stored</param>
            ICredentialsStep OnRemoteHost(string host);
        }

        public interface IBuildStep
        {
            /// <summary>
            /// Returns an instance of a UserConfiguration based on the parameters passed during the creation.
            /// </summary>
            UserConfiguration Build();
        }

        public interface ICredentialsStep
        {
            /// <param name="user">Username required to connect to remote machine</param>
            /// /// <param name="password">Password required to connect to remote machine</param>
            IBuildStep SetCredentials(string user, string password);
        }

        private class Steps : INameStep, IFileStep, IServerStep, IBuildStep, ICredentialsStep
        {
            private string _name;
            private string _host;
            private string _user;
            private string _password;
            private string _filePath;

            public IFileStep SetName(string name)
            {
                _name = name;
                return this;
            }

            public IServerStep SetFilePath(string filePath)
            {
                _filePath = filePath;
                return this;
            }

            public IBuildStep OnLocalhost()
            {
                _host = "localhost";
                return this;
            }

            public ICredentialsStep OnRemoteHost(string host)
            {
                _host = host;
                return this;
            }

            public UserConfiguration Build()
            {
                UserConfiguration userConfiguration = new UserConfiguration(_name, _filePath);

                return userConfiguration;
            }

            public IBuildStep SetCredentials(string user, string password)
            {
                _user = user;
                _password = password;
                return this;
            }
        }
    }
}