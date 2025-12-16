using HV.DAL.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace HV.DAL;

public sealed class UnitOfWork(
    ApplicationDbContext context)
    : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        if (_currentTransaction is not null)
            throw new InvalidOperationException("A database transaction is already active.");

        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("No active database transaction to commit.");

        await _context.SaveChangesAsync();
        await _currentTransaction.CommitAsync();
        await DisposeTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null)
            throw new InvalidOperationException("No active database transaction to roll back.");

        await _currentTransaction.RollbackAsync();
        await DisposeTransactionAsync();
    }

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

    private async Task DisposeTransactionAsync()
    {
        if (_currentTransaction is not null)
            await _currentTransaction.DisposeAsync();

        _currentTransaction = null;
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}