namespace LoLTournaments.Application.Services
{

    public interface IBootstrapCmd
    {
        public Task ExecuteAsync();
    }
    public class BootstrapService
    {
        public BootstrapService(IEnumerable<IBootstrapCmd> bootstrapCmds)
        {
            ExecuteCommandsAsync(bootstrapCmds);
        }

        private async void ExecuteCommandsAsync(IEnumerable<IBootstrapCmd> cmds)
        {
            if (cmds == null)
                return;
            
            foreach (var cmd in cmds)
            {
                await cmd.ExecuteAsync();
            }
        }
    }

}