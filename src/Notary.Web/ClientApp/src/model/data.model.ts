export abstract class Data {
    constructor() {
        this.active = false;
        this.created = new Date();
        this.createdBySlug = '';
        this.slug = '';
        this.updated = null;
        this.updatedBySlug = null;
    }

    /**
     * Boolean value to determine whether the current record is treated as active.
     */
    public active: boolean;

    /**
    * The date in which the current record was created
    */
    public created: Date;

    /**
    * The account slug of who created the record.
    */
    public createdBySlug: string;

    /**
    * The slug which uniquely identifies the record
    */
    public slug: string;

    /**
    * When the record was last updated
    */
    public updated: Date | null;

    /**
    * The account slug of who updated the record
    */
    public updatedBySlug: string | null;
}
