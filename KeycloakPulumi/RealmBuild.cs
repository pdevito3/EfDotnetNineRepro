namespace KeycloakPulumi;

using KeycloakPulumi.Extensions;
using KeycloakPulumi.Factories;
using Pulumi;
using Pulumi.Keycloak.Inputs;
using Keycloak = Pulumi.Keycloak;

class RealmBuild : Stack
{
    public RealmBuild()
    {
        var realm = new Keycloak.Realm("DevRealm-realm", new Keycloak.RealmArgs
        {
            RealmName = "DevRealm",
            RegistrationAllowed = true,
            ResetPasswordAllowed = true,
            RememberMe = true,
            EditUsernameAllowed = true
        });
        var thekitchencompanyScope = ScopeFactory.CreateScope(realm.Id, "the_kitchen_company");
        
        var recipeManagementPostmanMachineClient = ClientFactory.CreateClientCredentialsFlowClient(realm.Id,
            "recipe_management.postman.machine", 
            "974d6f71-d41b-4601-9a7a-a33081f84682", 
            "RecipeManagement Postman Machine",
            "https://oauth.pstmn.io");
        recipeManagementPostmanMachineClient.ExtendDefaultScopes("the_kitchen_company");
        recipeManagementPostmanMachineClient.AddAudienceMapper("the_kitchen_company");
        
        var recipeManagementPostmanCodeClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.postman.code", 
            "974d6f71-d41b-4601-9a7a-a33081f84680", 
            "RecipeManagement Postman Code",
            "https://oauth.pstmn.io",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementPostmanCodeClient.ExtendDefaultScopes("the_kitchen_company");
        recipeManagementPostmanCodeClient.AddAudienceMapper("the_kitchen_company");
        
        var recipeManagementSwaggerClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.swagger", 
            "974d6f71-d41b-4601-9a7a-a33081f80687", 
            "RecipeManagement Swagger",
            "https://localhost:5375",
            redirectUris: null,
            webOrigins: null
            );
        recipeManagementSwaggerClient.ExtendDefaultScopes("the_kitchen_company");
        recipeManagementSwaggerClient.AddAudienceMapper("the_kitchen_company");
        
        var recipeManagementBFFClient = ClientFactory.CreateCodeFlowClient(realm.Id,
            "recipe_management.bff", 
            "974d6f71-d41b-4601-9a7a-a33081f80688", 
            "RecipeManagement BFF",
            "https://localhost:4378",
            redirectUris: new InputList<string>() 
                {
                "https://localhost:4378/*",
                },
            webOrigins: new InputList<string>() 
                {
                "https://localhost:5375",
                "https://localhost:4378",
                }
            );
        recipeManagementBFFClient.ExtendDefaultScopes("the_kitchen_company");
        recipeManagementBFFClient.AddAudienceMapper("the_kitchen_company");
        
        var bob = new Keycloak.User("bob", new Keycloak.UserArgs
        {
            RealmId = realm.Id,
            Username = "bob",
            Enabled = true,
            Email = "bob@domain.com",
            FirstName = "Smith",
            LastName = "Bobson",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "bob",
                Temporary = true,
            },
        });

        var alice = new Keycloak.User("alice", new Keycloak.UserArgs
        {
            RealmId = realm.Id,
            Username = "alice",
            Enabled = true,
            Email = "alice@domain.com",
            FirstName = "Alice",
            LastName = "Smith",
            InitialPassword = new UserInitialPasswordArgs
            {
                Value = "alice",
                Temporary = true,
            },
        });
    }
}