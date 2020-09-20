export default interface ITodoManager {
    addTodoAsync(todo: ITodo): Promise<string>;
    getTodosAsync(): Promise<ITodo[]>;
}