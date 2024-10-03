using Microsoft.AspNetCore.Mvc;

namespace Osmismerka.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class WordSearchController : Controller
{
    private readonly (int x, int y)[] _directions =
    [
        (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1)
    ];

    // Model pre príjem matice a slov
    public class WordSearchRequest
    {
        public List<List<char>> Matrix { get; set; } = null!;
        public List<string> Words { get; set; } = null!;
    }
    
    [HttpPost]
    [ActionName(nameof(SearchWords))]
    public IActionResult SearchWords([FromBody] WordSearchRequest request)
    {
        // Ulozenie matice od uzivatela
        var matice = request.Matrix;
        var words = request.Words;
        
        var rows = matice.Count;
        var cols = matice.First().Count;
        
        // Maska najdenych pozic slov
        var foundMask = new bool[rows, cols];

        var matrix = ConvertInputMatrixToArray(matice, rows, cols);

        var foundWords = words.Where(word => NajdiSlovo(matrix, word.ToUpper(), foundMask, rows, cols)).ToList();

        // Generování tajenky
        var tajenka = GenerateTajenka(matrix, foundMask, rows, cols);

        // Vrácení nalezených slov a tajenky
        return Ok(new { foundWords, tajenka });
        
        // TODO error handling
    }
    
    // Funkcia na hladanie slova v matici
    private bool NajdiSlovo(char[,] matica, string slovo, bool[,] foundMask, int rows, int cols)
    {
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (matica[i, j] == slovo[0] && Hledej(matica, i, j, slovo, foundMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    // Funkcia na vyhladavanie slova vo vsetkych smeroch
    private bool Hledej(char[,] matica, int x, int y, string slovo, bool[,] foundMask)
    {
        var rows = matica.GetLength(0);
        var cols = matica.GetLength(1);
        var len = slovo.Length;

        foreach (var (dx, dy) in _directions)
        {
            int k, nx = x, ny = y;

            for (k = 0; k < len; k++)
            {
                if (nx < 0 || nx >= rows || ny < 0 || ny >= cols || matica[nx, ny] != slovo[k])
                {
                    break;
                }

                nx += dx;
                ny += dy;
            }

            if (k == len)
            {
                // Ak sme nasli cele slovo, oznacime jeho poziciu v maske
                nx = x;
                ny = y;
                for (k = 0; k < len; k++)
                {
                    foundMask[nx, ny] = true;
                    nx += dx;
                    ny += dy;
                }
                return true;
            }
        }
        return false;
    }
    
    private string GenerateTajenka(char[,] matrix, bool[,] foundMask, int rows, int cols)
    {
        var tajenka = new List<char>();

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                if (!foundMask[i, j])
                {
                    tajenka.Add(matrix[i, j]);
                }
            }
        }

        return new string(tajenka.ToArray());
    }

    private char[,] ConvertInputMatrixToArray(List<List<char>> matica, int rows, int cols)
    {
        var maticaData = new char[rows, cols];
        
        var row = 0;
        foreach (var rowData in matica)
        {
            var col = 0;
            foreach (var colData in rowData)
            {
                maticaData[row, col] = colData;
                col += 1;
            }
            row += 1;
        }

        return maticaData;
    }
}