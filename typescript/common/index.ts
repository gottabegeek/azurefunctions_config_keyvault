import { DefaultAzureCredential } from "@azure/identity";
import { SecretClient } from "@azure/keyvault-secrets";
import { CosmosClient, CosmosClientOptions } from "@azure/cosmos";
import TodoManager from "./todo.manager";
import ITodoManager from './itodo.manager';

const credential = new DefaultAzureCredential();
const keyVaultUri: string = process.env.KeyVaultUri;
const apiUrl: string = process.env.TodoDbUrl;
let dbKey: string = process.env.TodoDbKey;
let cosmosClient: CosmosClient;
let todoManager: ITodoManager = null;
const keyVaultClient: SecretClient = new SecretClient(keyVaultUri, credential);

const setupCosmosClient = async (): Promise<void> => {
    const latestSecret = await keyVaultClient.getSecret('TodoDbKey');
    dbKey = latestSecret.value;
    const clientOptions: CosmosClientOptions = { endpoint: apiUrl, key: dbKey };
    cosmosClient = new CosmosClient(clientOptions);
}

async function createTodoManager(): Promise<ITodoManager> {
    if (todoManager == null) {
        await setupCosmosClient();
        todoManager= new TodoManager(cosmosClient);
    }
    return todoManager;
}

export default createTodoManager;