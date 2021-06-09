import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { RegisterComponent } from '../register/register.component';

@Injectable()
export class PreventUnsaved implements CanDeactivate<RegisterComponent> {

    canDeactivate(component: RegisterComponent) {
        if (component.editForm.dirty && !component.editForm.valid) {
            return confirm('Jesteś pewien? Niezapisane zmiany zostaną utracone.');
        }
        return true;
    }
}