export interface User {
  id: number;
  name: string;
  email: string;
  role: 'ADMIN' | 'CONDUCTEUR' | 'CAISSIER' | 'COMPTABLE' | 'RH' | 'MAGASINIER';
  phone?: string;
  status: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface Chantier {
  id: number;
  code: string;
  name: string;
  type: string;
  wilaya: string;
  commune: string;
  address?: string;
  status: 'ETUDE' | 'TERRASSEMENT' | 'GROS_OEUVRES' | 'SECOND_OEUVRE' | 'FINITIONS' | 'LIVRE';
  startDate: string;
  endDatePlanned: string;
  budgetTotal: number;
  progressPercentage: number;
  surfaceTotaleM2?: number;
  nombreUnites?: number;
  chefChantierId?: number;
  chefChantierName?: string;
  effectifCount?: number;
  enginsCount?: number;
  caisseSolde?: number;
}

export interface Caisse {
  id: number;
  code: string;
  name: string;
  type: 'CHANTIER' | 'BANQUE' | 'CENTRALE';
  chantierId?: number;
  chantierName?: string;
  bankName?: string;
  rib?: string;
  balance: number;
  seuilAlerte: number;
  isUnderAlert?: boolean;
  status: string;
  responsableName?: string;
}

export interface CaisseMouvement {
  id: number;
  numeroBon: string;
  caisseId: number;
  caisseName?: string;
  type: 'ALIMENTATION' | 'DECAISSEMENT' | 'VIREMENT';
  montant: number;
  dateMouvement: string;
  motif: string;
  rubrique?: string;
  beneficiaireName?: string;
  pieceJustificativeRef?: string;
  modePaiement: string;
  authorName?: string;
}

export interface DashboardSummary {
  totalBudgetChantiers: number;
  totalTresorerieGlobale: number;
  totalDepensesMois: number;
  chantiersActifsCount: number;
  totalEffectifOuvriers: number;
  alertesCaissesCount: number;
}
