// import { NgModule } from '@angular/core';
// import { Routes, RouterModule } from '@angular/router';
// import { ContactDetailComponent } from './detail/detail.component';
// import { AuthGuard } from 'src/app/shared/guards/auth-guard';
// import { MasterParticipantGuard } from 'src/app/shared/guards/master-participant-guard';
// import { Authorization } from 'src/app/shared/models/authorization';
// import { ContactsListPageComponent } from './list-page/list-page.component';

// const routes: Routes = [
//   {
//     path: '',
//     component: ContactsListPageComponent,
//     children: [
//       {
//         path: 'details/:id',
//         component: ContactDetailComponent,
//         canActivate: [AuthGuard, MasterParticipantGuard],
//         data: { guards: ['CoreAccessGuard', 'ParticipantGuard'], authorizations: [Authorization.canAccessContactsApp_Edit] }
//       }
//     ]
//   }
// ];

// @NgModule({
//   imports: [RouterModule.forChild(routes)],
//   exports: [RouterModule]
// })
// export class ContactsRoutingModule {}
