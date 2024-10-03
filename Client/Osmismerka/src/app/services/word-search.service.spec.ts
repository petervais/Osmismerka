import { TestBed } from '@angular/core/testing';

import { WordSearchService } from './word-search.service';

describe('WordSearchService', () => {
  let service: WordSearchService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WordSearchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
