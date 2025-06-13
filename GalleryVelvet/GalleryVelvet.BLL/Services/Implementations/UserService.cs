using GalleryVelvet.BLL.DTOs.User;
using GalleryVelvet.BLL.Services.Interfaces;
using GalleryVelvet.DAL.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace GalleryVelvet.BLL.Services.Implementations;

public sealed class UserService(
    IUserRepository userRepository,
    IOrderRepository orderRepository) : IUserService
{
    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken: cancellationToken);
        
        if (user is null)
        {
            throw new InvalidOperationException("Пользователь не найден");
        }

        return user.Adapt<UserProfileDto>();
    }

    public async Task<UserProfileDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileDto updateDto, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken: cancellationToken);
        
        if (user is null)
        {
            throw new InvalidOperationException("Пользователь не найден");
        }
        
        if (user.Login != updateDto.Login)
        {
            var existingUser = await userRepository.GetFirstOrDefaultAsync(
                u => u.Login == updateDto.Login && u.Id != userId, 
                cancellationToken: cancellationToken);
            
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Пользователь с таким логином уже существует");
            }
        }
        
        if (!string.IsNullOrWhiteSpace(updateDto.Email) && user.Email != updateDto.Email)
        {
            var existingUser = await userRepository.GetFirstOrDefaultAsync(
                u => u.Email == updateDto.Email && u.Id != userId, 
                cancellationToken: cancellationToken);
            
            if (existingUser is not null)
            {
                throw new InvalidOperationException("Пользователь с таким email уже существует");
            }
        }
        
        user.Login = updateDto.Login;
        user.FirstName = updateDto.FirstName;
        user.LastName = updateDto.LastName;
        user.Email = updateDto.Email;
        user.PhoneNumber = updateDto.PhoneNumber;

        await userRepository.UpdateAsync(user, cancellationToken);

        return user.Adapt<UserProfileDto>();
    }

    public async Task<IEnumerable<UserOrderDto>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var orders = await orderRepository
            .GetQueryable(
                filter: o => o.UserId == userId,
                orderBy: q => q.OrderByDescending(o => o.OrderDate),
                includeBuilder: q => q
                    .Include(o => o.OrderStatus)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                            .ThenInclude(p => p.Images)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Size))
            .ToListAsync(cancellationToken);

        return orders.Adapt<IEnumerable<UserOrderDto>>();
    }
}