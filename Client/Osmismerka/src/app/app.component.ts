import { Component } from '@angular/core';
import { WordSearchService } from './services/word-search.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Osmismerka';

  matrixInput: string = '';
  wordsInput: string = '';
  results: any[] = [];
  tajenka: string = '';

  constructor(private wordSearchService: WordSearchService) { }

  // Funkcia pre spracovanie matice a vyhladanie slov
  search() {
    const matrix = this.convertMatrixInputToArray(this.matrixInput);
    const words = this.wordsInput.split(' ').filter(word => word.length > 0);

    this.wordSearchService.searchWords(matrix, words).subscribe(res => {
      this.results = res.foundWords;
      this.tajenka = res.tajenka;
    });
  }

  // Konverzia matice zadanu uzivatelom na pole poli
  private convertMatrixInputToArray(input: string): string[][] {
    return input.split('\n').map(row => row.trim().split(' '));
  }
}
