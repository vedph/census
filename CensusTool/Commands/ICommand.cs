using System.Threading.Tasks;

namespace CensusTool.Commands
{
    public interface ICommand
    {
        Task Run();
    }
}
