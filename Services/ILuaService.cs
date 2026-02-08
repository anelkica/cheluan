using cheluan.Models;

namespace cheluan.Services;

public interface ILuaService
{
    Result<string> ReadScript(string filePath);
    Result ExecuteCode(string code);
}
