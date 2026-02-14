using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using cheluan.Models;

namespace cheluan.Services;

public interface ILuaService
{
    Result ExecuteCode(string code);
    Task<Result<string>> ReadScriptFileAsync(IStorageFile file);
    Task<Result> SaveScriptFileAsync(IStorageFile file, string content);
}