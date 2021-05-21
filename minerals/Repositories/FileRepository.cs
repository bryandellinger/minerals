using Microsoft.EntityFrameworkCore;
using Minerals.Contexts;
using Minerals.Interfaces;
using Minerals.ViewModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Minerals.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly DataContext context;
        public FileRepository(DataContext ctx) => context = ctx;

        public async Task<object> GetFilesByCheckAsync(long id)
        {
            if (id > 0)
            {
                return (await context.Checks
                  .Include(x => x.Files)
                  .FirstAsync(x => x.Id == id)
                  .ConfigureAwait(false))
                  .Files.ToList();
            }
            else
            {
                return new List<File>();
            }
        }

        public async Task<object> GetFilesByUnitAsync(long id)
        {
            if (id > 0)
            {
                return (await context.Units
                  .Include(x => x.Files)
                  .FirstAsync(x => x.Id == id)
                  .ConfigureAwait(false))
                  .Files.ToList();
            } else
            {
                return new List<File>();
            }
        }

        public async Task<object> GetFilesByUploadPaymentAsync(long id)
        {
            {
                if (id > 0)
                {
                    return (await context.UploadPayments
                      .Include(x => x.Files)
                      .FirstAsync(x => x.Id == id)
                      .ConfigureAwait(false))
                      .Files.ToList();
                }
                else
                {
                    return new List<File>();
                }
            }
        }

        public async Task<object> GetFilesByUploadTemplateAsync(long id)
        {
            if (id > 0)
            {
                return (await context.UploadTemplates
                  .Include(x => x.Files)
                  .FirstAsync(x => x.Id == id)
                  .ConfigureAwait(false))
                  .Files.ToList();
            }
            else
            {
                return new List<File>();
            }
        }
    }
}
