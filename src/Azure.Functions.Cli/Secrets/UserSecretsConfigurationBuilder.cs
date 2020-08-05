using System.Collections.Generic;
using Azure.Functions.Cli.Common;
using Azure.Functions.Cli.Helpers;
using Microsoft.Azure.WebJobs.Script;
using Microsoft.Extensions.Configuration;

namespace Azure.Functions.Cli.Diagnostics
{
    internal class UserSecretsConfigurationBuilder : IConfigureBuilder<IConfigurationBuilder>
    {
        private readonly string _scriptPath;

        public UserSecretsConfigurationBuilder(string scriptPath)
        {
            _scriptPath = scriptPath;
        }

        public void Configure(IConfigurationBuilder builder)
        {
            string userSecretsId = GetUserSecretsId(_scriptPath);
            if (userSecretsId == null) return;

            builder.AddUserSecrets(userSecretsId);
        }

        private string GetUserSecretsId(string scriptPath)
        {
            string projectFilePath = ProjectHelpers.FindProjectFile(scriptPath);
            if (projectFilePath == null) return null;

            var projectRoot = ProjectHelpers.GetProject(projectFilePath);
            var userSecretsId = ProjectHelpers.GetPropertyValue(projectRoot, Constants.UserSecretsIdElementName);

            return userSecretsId;
        }
    }
}