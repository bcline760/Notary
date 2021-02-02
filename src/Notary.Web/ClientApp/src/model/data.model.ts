export interface Data {
    /**
     * Boolean value to determine whether the current record is treated as active.
     */
    active: boolean;

    /**
    * The date in which the current record was created
    */
    created: Date;

    /**
    * The account slug of who created the record.
    */
    createdBySlug: string;

    /**
    * The slug which uniquely identifies the record
    */
    slug: string;

    /**
    * When the record was last updated
    */
    updated: Date | null;

    /**
    * The account slug of who updated the record
    */
    updatedBySlug: string | null;
}
