using Avalonia.Platform.Storage;
using cheluan.Models;
using System;
using System.Threading.Tasks;

namespace cheluan.Services;

public interface ILuaService
{
    Result ExecuteCode(string code);
    Task<Result<string>> ReadScriptFileAsync(IStorageFile file);
    Task<Result> SaveScriptFileAsync(IStorageFile file, string content);

    public void RegisterSpawner(Func<Turtle> spawner);
}