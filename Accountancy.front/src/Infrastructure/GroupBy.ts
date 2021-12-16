export class Group<TKey, TItem> {
    key!: TKey;
    items!: TItem[];
}

export const groupBy = <TItem, TKey>(list: TItem[], keyGetter: (item: TItem) => TKey): Group<TKey, TItem>[] => {
    const groups: Group<TKey, TItem>[] = [];
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = groups.filter(x => x.key === key)[0];
        if (!collection) {
            groups.push({
                key,
                items: [item]
            });
        } else {
            collection.items.push(item);
        }
    });
    return groups;
}
