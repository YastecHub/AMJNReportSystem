using AMJNReportSystem.Application.Wrapper;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace AMJNReportSystem.Persistence.Extensions
{
    public static class MapperExtensions
    {
        public static async Task<PaginatedResult<TDto>> ToMappedPaginatedResultAsync<T, TDto>(
            this IQueryable<T> query, int pageNumber, int pageSize)
            where T : class
        {
            var converter = new MappedPaginatedResultConverter<T, TDto>(pageNumber, pageSize);
            return await converter.ConvertBackAsync(query);
        }

        public static async Task<PaginatedResult<TDto>> ToMappedPaginatedResultAsync<T, TDto>(
           this IList<T> query, int pageNumber, int pageSize)
           where T : class
        {
            var converter = new MappedPaginatedResultConverter<T, TDto>(pageNumber, pageSize);
            return await converter.ConvertBackAsync(query);
        }

        public class MappedPaginatedResultConverter<T, TDto> : IMapsterConverterAsync<PaginatedResult<TDto>, IQueryable<T>>
            where T : class
        {
            private int _pageNumber;
            private int _pageSize;

            public MappedPaginatedResultConverter(int pageNumber, int pageSize)
            {
                _pageNumber = pageNumber;
                _pageSize = pageSize;
            }

            public Task<IQueryable<T>> ConvertAsync(PaginatedResult<TDto> item)
            {
                throw new NotImplementedException();
            }

            public async Task<PaginatedResult<TDto>> ConvertBackAsync(IQueryable<T> query)
            {
                if (query == null)
                {
                    throw new ArgumentNullException(nameof(query));
                }

                _pageNumber = _pageNumber == 0 ? 1 : _pageNumber;
                _pageSize = _pageSize == 0 ? 10 : _pageSize;
                List<string> columnValues = new();

                int count = await query.AsNoTracking().CountAsync();
                _pageNumber = _pageNumber <= 0 ? 1 : _pageNumber;
                var items = await query.Skip((_pageNumber - 1) * _pageSize).Take(_pageSize).AsNoTracking().ToListAsync();
                var mappedItems = items.Adapt<List<TDto>>();
                return await Task.FromResult(PaginatedResult<TDto>.Success(mappedItems, count, _pageNumber, _pageSize));
            }
            public async Task<PaginatedResult<TDto>> ConvertBackAsync(IList<T> query)
            {
                if (query == null)
                {
                    throw new ArgumentNullException(nameof(query));
                }

                _pageNumber = _pageNumber == 0 ? 1 : _pageNumber;
                _pageSize = _pageSize == 0 ? 10 : _pageSize;
                int count = query.Count;
                _pageNumber = _pageNumber <= 0 ? 1 : _pageNumber;
                var items = query.Skip((_pageNumber - 1) * _pageSize).Take(_pageSize).ToList();
                var mappedItems = items.Adapt<List<TDto>>();
                return await Task.FromResult(PaginatedResult<TDto>.Success(mappedItems, count, _pageNumber, _pageSize));
            }
        }
    }
}