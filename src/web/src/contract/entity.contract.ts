/** Common properties used throughout most contracts */
export interface Entity {
    /** Get or set whether the entity is active */
    active: boolean;

    /** Get or set when created */
    created: Date;

    /** Get or set who created entity */
    createdBySlug: string;

    /** Get or set the slug identifier */
    slug: string;

    /** Get or set when last updated */
    updated?: Date;

    /** Get or set last updating user */
    updatedBySlug?: string;
}
