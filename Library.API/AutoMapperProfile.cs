using AutoMapper;
using Library.BLL.DTOs;
using Library.DAL.Entities;

namespace Library.API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Count() > 0 ? x.Ratings.ToList().Average(r => r.Score) : 0))
                .ForMember(x => x.ReviewsNumber, opt => opt.MapFrom(x => x.Reviews.Count()));

            CreateMap<Review, ReviewDTO>();

            CreateMap<Book, BookDetailsDTO>()
                .ForMember(x => x.Rating, opt => opt.MapFrom(x => x.Ratings.Count() > 0 ? x.Ratings.ToList().Average(r => r.Score) : 0));

            CreateMap<BookRequestDTO, Book>();
            CreateMap<ReviewRequestDTO, Review>();
            CreateMap<RatingRequestDTO, Rating>();
        }
    }
}
