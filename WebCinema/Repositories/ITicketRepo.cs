using WebCinema.Models;

namespace WebCinema.Repositories
{
    public interface ITicketRepo
    {
        Task AddTicketAsync(Ticket ticket);
        Task AddTicketInfosAsync(IEnumerable<TicketInfo> ticketInfos, int ticketId);
        Task AddTicketCombosAsync(IEnumerable<TicketCombo> ticketCombos, int ticketId);
        List<string> GetBookedSeats(int showtimeId);
    }
}
