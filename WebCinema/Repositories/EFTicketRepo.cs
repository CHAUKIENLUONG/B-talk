using WebCinema.Models;

namespace WebCinema.Repositories
{
    public class EFTicketRepo : ITicketRepo
    {
        private readonly ApplicationDbContext _context;
        public EFTicketRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTicketAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AddTicketInfosAsync(IEnumerable<TicketInfo> ticketInfos, int ticketId)
        {
            foreach (var ticketInfo in ticketInfos)
            {
                ticketInfo.TicketID = ticketId;
            }
            await _context.TicketInfos.AddRangeAsync(ticketInfos);
            await _context.SaveChangesAsync();
        }

        public async Task AddTicketCombosAsync(IEnumerable<TicketCombo> ticketCombos, int ticketId)
        {
            foreach (var ticketCombo in ticketCombos)
            {
                ticketCombo.TicketId = ticketId;
            }
            await _context.TicketCombos.AddRangeAsync(ticketCombos);
            await _context.SaveChangesAsync();
        }

        public List<string> GetBookedSeats(int showtimeId)
        {
            var bookedSeatIds = _context.TicketInfos
                .Where(t => t.Ticket.ShowId == showtimeId && t.State == true)
                .Select(t => t.SeatId)
                .ToList();
            return bookedSeatIds;
        }
    }
}
