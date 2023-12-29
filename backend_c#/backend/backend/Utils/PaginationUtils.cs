using Microsoft.AspNetCore.Mvc.RazorPages;

namespace backend.Utils {
    public class PaginationUtils {
        public string? GetNextUrl(int currentPage, int totalPages, string baseUrl) {
            return _HasNextPage(currentPage, totalPages) ? $"{baseUrl}?page={currentPage + 1}": null;
        }

        public string? GetPreviousUrl(int currentPage, string baseUrl) {

            return _HasPreviousPage(currentPage) ? $"{baseUrl}?page={currentPage-1}" : null;
        }

        private bool _HasNextPage(int currentPage, int totalPages) {
            return currentPage < totalPages - 1;
        }

        private bool _HasPreviousPage(int currentPage) {
            return currentPage > 0;
        }
    }
}
