using System.Threading;
using System.Threading.Tasks;
using CqrsApi.Data.Context;
using CqrsApi.Requests.CommandResponses;
using CqrsApi.Requests.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CqrsApi.Requests.CommandHandlers
{
    public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, DeleteMovieSuccessResponse>
    {
        private readonly PostgreContext _context;

        public DeleteMovieHandler(PostgreContext context)
        {
            _context = context;
        }

        public async Task<DeleteMovieSuccessResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(x => x.MovieId == request.MovieId, cancellationToken);
            if (movie == null)
                return null;
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync(cancellationToken);
            return new DeleteMovieSuccessResponse(request.MovieId);
        }
    }
}