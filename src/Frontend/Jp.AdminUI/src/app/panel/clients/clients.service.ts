import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

import { Client, ClientSecret } from "../../shared/viewModel/client.model";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { DefaultResponse } from "../../shared/viewModel/default-response.model";
import { ClientList } from "../../shared/viewModel/client-list.model";

@Injectable()
export class ClientService {




    constructor(private http: HttpClient) {
    }


    public getClients(): Observable<DefaultResponse<ClientList[]>> {
        return this.http.get<DefaultResponse<ClientList[]>>(environment.ResourceServer + "clients/list");
    }

    public getClientDetails(clientId: string): Observable<DefaultResponse<Client>> {
        let options = {
            params: {
                clientId: clientId
            }
        };
        return this.http.get<DefaultResponse<Client>>(environment.ResourceServer + "clients/details", options);
    }

    public update(model: Client): Observable<DefaultResponse<boolean>> {
        return this.http.post<DefaultResponse<boolean>>(environment.ResourceServer + "clients/update", model);
    }

    public getClientSecrets(clientId: string): Observable<DefaultResponse<ClientSecret[]>> {
        let options = {
            params: {
                clientId: clientId
            }
        };
        return this.http.get<DefaultResponse<ClientSecret[]>>(environment.ResourceServer + "clients/secrets", options);
    }
    public removeSecret(client: string, id: number): Observable<DefaultResponse<boolean>> {
        const removeCommand = {
            id: id,
            clientId: client
        };
        return this.http.post<DefaultResponse<boolean>>(environment.ResourceServer + "clients/remove-secret", removeCommand);
    }

    public saveSecret(model: ClientSecret): Observable<DefaultResponse<boolean>> {
        return this.http.post<DefaultResponse<boolean>>(environment.ResourceServer + "clients/save-secret", model);
    }
}
