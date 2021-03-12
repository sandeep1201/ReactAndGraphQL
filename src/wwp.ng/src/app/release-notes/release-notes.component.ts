import { Component, OnInit } from '@angular/core';

declare var $: any;

@Component({
  selector: 'app-release-notes',
  templateUrl: './release-notes.component.html',
  styleUrls: ['./release-notes.component.css']
})
export class ReleaseNotesComponent implements OnInit {
  editMode = false;
  constructor() {}

  ngOnInit() {
    // jQuery for scrolling to sections
    $(document).on('click', '#version-table td, .totop', function() {
      const sec = $(this).data('goto');
      $('html, body').stop();
      if (sec) {
        $('html, body').animate(
          {
            scrollTop: $('#' + sec).offset().top - 75
          },
          1000
        );
      }
    });
  }
}
