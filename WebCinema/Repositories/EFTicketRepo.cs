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
            // Add the ticket to the context
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync(); // Save to generate TicketId
        }

        public async Task AddTicketInfosAsync(IEnumerable<TicketInfo> ticketInfos, int ticketId)
        {
            // Associate TicketInfos with the ticket using the provided TicketId
            foreach (var ticketInfo in ticketInfos)
            {
                ticketInfo.TicketID = ticketId;
            }

            // Add TicketInfos to the context
            await _context.TicketInfos.AddRangeAsync(ticketInfos);
            await _context.SaveChangesAsync(); // Save changes
        }

        public async Task AddTicketCombosAsync(IEnumerable<TicketCombo> ticketCombos, int ticketId)
        {
            // Associate TicketCombos with the ticket using the provided TicketId
            foreach (var ticketCombo in ticketCombos)
            {
                ticketCombo.TicketId = ticketId;
            }

            // Add TicketCombos to the context
            await _context.TicketCombos.AddRangeAsync(ticketCombos);
            await _context.SaveChangesAsync(); // Save changes
        }

        public List<string> GetBookedSeats(int showtimeId)
        {
            // Retrieve booked seat IDs for the given showtime
            var bookedSeatIds = _context.TicketInfos
                .Where(t => t.Ticket.ShowId == showtimeId && t.State == true)
                .Select(t => t.SeatId)
                .ToList();
            return bookedSeatIds;
        }
    }
}
